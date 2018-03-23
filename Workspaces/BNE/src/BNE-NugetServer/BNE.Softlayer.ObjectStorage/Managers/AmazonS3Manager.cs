using Amazon.S3;
using Amazon.S3.Model;
using BNE.StorageManager.Config;
using BNE.StorageManager.Managers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace BNE.StorageManager
{
    public class AmazonS3Manager : IFileManager, IFileConfig
    {

        private IAmazonS3 _client;
        public string _bucket { get; set; }
        public string _folder { get; set; }
        public bool _enableCulture { get; set; }
        private Regex _regexToPath;

        public void LoadConfig(StorageConfiguration storage)
        {
            try
            {
                StoragesConfigSection config = System.Configuration.ConfigurationManager.GetSection("StoragesConfig") as StoragesConfigSection;

                var bucketConfig = config.AmazonS3Buckets[storage.StorageName];

                var s3Config = new AmazonS3Config()
                {
                    ServiceURL = bucketConfig.RegionEndpoint,
                };

                _client = new AmazonS3Client(bucketConfig.AccessKeyID, bucketConfig.SecretAccessKey, s3Config);
                _bucket = bucketConfig.Bucket;
                _folder = storage.StorageFolder;
                _enableCulture = config.Folders[_folder].EnableCulture;

                if (!String.IsNullOrEmpty(storage.RegexPath))
                {
                    _regexToPath = new Regex(storage.RegexPath);
                }
            }
            catch (Exception ex)
            {
                throw new ConfigurationErrorsException("Ocorreu uma falha ao tentar configurar o Object Storage", ex);
            }
        }

        public bool Save(string objectPath, byte[] byteArray)
        {
            bool _result = false;
            try 
            {
                objectPath = ResolvePath(objectPath);

                //---------- Client & Filename
                string fileName = objectPath.Split('/').Last();

                //---------- Put object on S#
                PutObjectRequest request = new PutObjectRequest()
                {
                    BucketName = this._bucket,
                    Key = objectPath
                };
                request.InputStream = new System.IO.MemoryStream();
                request.InputStream.Write(byteArray, 0, byteArray.Length);

                PutObjectResponse response2 = _client.PutObject(request);

                _result = true;

            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                throw amazonS3Exception;
            }
            return _result;
        }

        public bool Delete(string objectPath)
        {
            objectPath = ResolvePath(objectPath);

            bool _result = false;
            try
            {
                DeleteObjectRequest deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = this._bucket,
                    Key = objectPath
                };
                _client.DeleteObject(deleteObjectRequest);

            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                throw amazonS3Exception;
            }
            return _result;
        }

        public bool Exists(string objectPath)
        {
            objectPath = ResolvePath(objectPath);

            bool _result = false;
            try
            {
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = this._bucket,
                    Key = objectPath
                };

                using (GetObjectResponse response = _client.GetObject(request))
                {
                    _result = true;
                }
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode.Equals("NoSuchKey"))
                    _result = false;
                else
                    throw amazonS3Exception;
            }
            return _result;

        }

        public async Task<bool> ExistsAsync(string objectPath)
        {
            objectPath = ResolvePath(objectPath);

            bool _result = false;
            try
            {
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = this._bucket,
                    Key = objectPath
                };

                using (GetObjectResponse response = _client.GetObject(request))
                {
                    _result = true;
                }
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode.Equals("NoSuchKey"))
                    _result = false;
                else
                    throw amazonS3Exception;
            }
            return await Task.FromResult(_result);

        }

        public byte[] GetBytes(string objectPath)
        {
            objectPath = ResolvePath(objectPath);

            try
            {
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = this._bucket,
                    Key = objectPath
                };
                
                using (GetObjectResponse response = _client.GetObject(request))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        response.ResponseStream.CopyTo(memoryStream);
                        return memoryStream.ToArray();
                    }
                }
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                throw amazonS3Exception;
            }
        }

        public bool Copy(string objectOriginPath, string objectDestinationPath)
        {
            objectOriginPath = ResolvePath(objectOriginPath);
            objectDestinationPath = ResolvePath(objectDestinationPath, false);

            bool _result = false;
            try
            {
                this.Save(objectDestinationPath, this.GetBytes(objectOriginPath));
                _result = true;
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                throw amazonS3Exception;
            }
            return _result;
        }

        public bool Move(string objectOriginPath, string objectDestinationPath)
        {
            objectOriginPath = ResolvePath(objectOriginPath);
            objectDestinationPath = ResolvePath(objectDestinationPath, false);

            bool _result = false;
            try
            {
                this.Copy(objectOriginPath, objectDestinationPath);
                this.Delete(objectOriginPath);
                _result = true;
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                throw amazonS3Exception;
            }
            return _result;
        }

        public string GetUrl(string objectPath)
        {
            throw new NotImplementedException();
        }

        private Amazon.RegionEndpoint GetRegion(string region)
        {
            switch (region)
            {
                //return Amazon.RegionEndpoint.CNNorth1
                case "ap-northeast-1": return Amazon.RegionEndpoint.APNortheast1;
                case "ap-northeast-2": return Amazon.RegionEndpoint.APNortheast2;
                case "ap-south-1": throw new NotImplementedException("ap-south-1 nao implementado no SDK da Amazon");
                case "ap-southeast-1": return Amazon.RegionEndpoint.APSoutheast1;
                case "ap-southeast-2": return Amazon.RegionEndpoint.APSoutheast2;
                case "eu-central-1": return Amazon.RegionEndpoint.EUCentral1;
                case "eu-west-1": return Amazon.RegionEndpoint.EUWest1;
                case "sa-east-1": return Amazon.RegionEndpoint.SAEast1;
                case "us-east-1": return Amazon.RegionEndpoint.USEast1;
                case "us-east-2": throw new NotImplementedException("us-east-2 nao implementado no SDK da Amazon");
                case "us-west-1": return Amazon.RegionEndpoint.USWest1;
                case "us-west-2": return Amazon.RegionEndpoint.USWest2;
                default:
                    throw new NotImplementedException("Region {region} não é uma region da amazon."); ;
            }
        }

        /// <summary>
        /// Implementa o tratamento ao tratamento da url incluindo as pastas a serem criadas com os grupos da Regex
        /// </summary>
        /// <param name="objectPath">Caminho do arquivo</param>
        /// <returns></returns>
        public String ResolvePath(String objectPath, bool includeURL = true)
        {
            objectPath = HttpUtility.UrlEncode(objectPath);

            if(_enableCulture)
                return Helper.CombineUrl(Thread.CurrentThread.CurrentCulture.ToString() + "/" + _folder, objectPath);
            else
                return Helper.CombineUrl(_folder, objectPath);

            //if (_regexToPath == null)
            //{
            //if (includeURL)
            //        return Helper.CombineUrl(_folder, objectPath);
            //    else
            //        return Helper.CombineUrl(_folder, objectPath);
            //}

            //String folderPath;
            //if (includeURL)
            //    return Helper.CombineUrl(_containerUrl, _folder, Helper.ResolvePath(objectPath, '/', _regexToPath, out folderPath));
            //else
            //    return Helper.CombineUrl(_container, _folder, Helper.ResolvePath(objectPath, '/', _regexToPath, out folderPath));
        }

        public IEnumerable<string> List()
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            throw new NotImplementedException();
        }
    }
}
