//using System.Dynamic;
//using System.Linq;
//using System.Xml.Linq;

//namespace BNE.Services.Plugins
//{
//    public class DynamicXml : DynamicObject
//    {
//        private readonly XElement _root;

//        private DynamicXml(XElement root)
//        {
//            _root = root;
//        }

//        public static DynamicXml Parse(string xmlString)
//        {
//            return new DynamicXml(XDocument.Parse(xmlString).Root);
//        }

//        public static DynamicXml Load(string filename)a
//        {
//            return new DynamicXml(XDocument.Load(filename).Root);
//        }

//        public override bool TryGetMember(GetMemberBinder binder, out object result)
//        {
//            result = null;

//            var att = _root.Attribute(binder.Name);
//            if (att != null)
//            {
//                result = att.Value;
//                return true;
//            }

//            var nodes = _root.Elements(binder.Name).ToList();
//            if (nodes.Any())
//            {
//                result = nodes.Select(n => new DynamicXml(n)).ToList();
//                return true;
//            }

//            var node = _root.Element(binder.Name);
//            if (node != null)
//            {
//                if (node.HasElements)
//                    result = new DynamicXml(node);
//                else
//                    result = node.Value;
//                return true;
//            }

//            return true;
//        }
//    }
//}