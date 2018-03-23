using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Web.Script.Serialization;
using BNE.Log.Base;

namespace BNE.Log
{
    internal class BufferLog
    {
        private static readonly Queue<BaseMessage> LsBuffer = new Queue<BaseMessage>(1000);
        private static readonly Configuration ObjConfig;
        private static readonly Thread ObjThread;
        private static bool _atualizaBuffer = true;

        ~BufferLog()
        {
            GravarLogDB();
        }

        static BufferLog()
        {
            ObjConfig = ConfigurationManager.GetSection("BNE.Log.Configuration") as Configuration ?? new Configuration();

            if (string.IsNullOrEmpty(ObjConfig.TypeLibraryDbWrites))
            {
                var ass = Assembly.Load("BNE.Log.ADO"); //Padrão
                ReadWriterBd = ass.CreateInstance("BNE.Log.ADO.ReadWriteDbLog") as IReadWriteDbLog;
            }
            else
            {
                var ar = ObjConfig.TypeLibraryDbWrites.Split(',');

                var ass = Assembly.Load(ar[1].Trim());
                ReadWriterBd = ass.CreateInstance(ar[0].Trim()) as IReadWriteDbLog;
            }

            if (ObjThread == null)
            {
                ObjThread = new Thread(TimeOutLog);
                ObjThread.Start();
            }
        }

        public static void AddLog(BaseMessage log)
        {
            lock (LsBuffer)
            {
                LsBuffer.Enqueue(log);

                if (LsBuffer.Count >= ObjConfig.Tamanho)
                    GravarLogDB();
            }
        }

        #region Stop
        /// <summary>
        /// Chamar para parar a Thread que atualiza o buffer.
        /// Usar somente quando for encerar o processo.
        /// </summary>
        public static void Stop()
        {
            lock (LsBuffer)
            {
                GravaBuffer();
                _atualizaBuffer = false;
            }

            try
            {
                if (ObjThread.IsAlive &&
                    (ObjThread.ThreadState & System.Threading.ThreadState.WaitSleepJoin) == System.Threading.ThreadState.WaitSleepJoin)
                    ObjThread.Abort();
            }
            catch { }
        }
        #endregion

        private static void TimeOutLog()
        {
            while (_atualizaBuffer)
            {
                Thread.Sleep(ObjConfig.Timeout);

                GravaBuffer();
            }
        }

        private static void GravaBuffer()
        {
            try
            {
                GravarLogDB();
            }
            catch (Exception ex)
            {
                try
                {
                    Helper.GravarLogDisco(ex);
                }
                catch { }
            }
        }

        static readonly IReadWriteDbLog ReadWriterBd;
        internal static void GravarLogDB()
        {
            lock (LsBuffer)
            {
                try
                {
                    ReadWriterBd.WriteList(LsBuffer);
                }
                catch (Exception ex)
                {
                    try
                    {
                        GravarLogEventViewer(ex);
                    }
                    catch (Exception ex1)
                    {
                        try
                        {
                            Helper.GravarLogDisco(ex1);
                        }
                        catch { }
                    }
                }
                finally
                {
                    LsBuffer.Clear();
                }
            }
        }

        internal static void GravarLogEventViewer(Exception ex)
        {
            EventLogPermission eventLogPerm = new EventLogPermission(EventLogPermissionAccess.Administer, ".");
            eventLogPerm.PermitOnly();

            string eventLog = "Application";
            string eventSource = "ASP.NET 2.0.50727.0";

            EventLog evLog = new EventLog(eventLog) { Source = eventSource };

            evLog.WriteEntry(ex.Message, EventLogEntryType.Error);

            //TODO: Talvez precise truncar essas mensagens.
            foreach (BaseMessage log in LsBuffer)
            {
                evLog.WriteEntry("Log: " + new JavaScriptSerializer().Serialize(log), EventLogEntryType.Error);
            }

            evLog.Close();
        }

        

        //internal static ErrorMessage GetError(Guid id)
        //{
        //    lock (LsBuffer)
        //    {
        //        if (LsBuffer.Count(t => t.Id == id) > 0)
        //            return LsBuffer.First(t => t.Id == id);
        //    }

        //    return ReadWriterBd.GetError(id);
        //}

        //internal static ErrorMessage GetLastError(Guid id)
        //{
        //    lock (LsBuffer)
        //    {
        //        var erro = LsBuffer.FirstOrDefault(t => t.Id == id);

        //        if (erro != null)
        //        {
        //            while (erro != null && erro.InnerException != null)
        //            {
        //                var lastError = erro.InnerException;
        //                erro = lastError;
        //            }

        //            return erro;
        //        }
        //    }

        //    return ReadWriterBd.GetLastError(id);
        //}
    }
}
