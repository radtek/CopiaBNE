using System.IO;
using System.Web;
using System.Web.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BNE.Componentes.Test
{
    [TestClass]
    public class ImageSlicerTest
    {
        private HttpContext _context;
        [TestInitialize]
        public void TestInit()
        {
            SimpleWorkerRequest request = new SimpleWorkerRequest("", "", "", null, new StringWriter());
            _context = new HttpContext(request);
        }

        [TestMethod]
        public void CreateThumbnailTest__ShouldPass_100x50()
        {
            HttpContext.Current = _context;

            var imageUrl = "http://www.bne.com.br/img/logo_bne.gif";

            var thumbnail = new ImageSlicer() { ThumbDir = "/thumbs/", ImageUrl = imageUrl }.CreateThumbnail(0, 100, 0, 50, 12, 12, imageUrl);

            Assert.AreEqual(true, !string.IsNullOrWhiteSpace(thumbnail));
        }

        [TestMethod]
        public void CreateThumbnailTest_ShouldPass_355x60()
        {
            HttpContext.Current = _context;

            var imageUrl = "http://www.bne.com.br/img/logo_bne.gif";

            var thumbnail = new ImageSlicer() { ThumbDir = "/thumbs/", ImageUrl = imageUrl }.CreateThumbnail(0, 355, 0, 60, 12, 12, "http://www.bne.com.br/img/logo_bne.gif");

            Assert.AreEqual(true, !string.IsNullOrWhiteSpace(thumbnail));
        }

        [TestMethod]
        public void CreateThumbnailTest_ShouldPass_120x40()
        {
            HttpContext.Current = _context;

            var imageUrl = "http://www.bne.com.br/img/logo_bne.gif";

            var thumbnail = new ImageSlicer() { ThumbDir = "/thumbs/", ImageUrl = imageUrl }.CreateThumbnail(35, 120, 15, 40, 12, 12, "http://www.bne.com.br/img/logo_bne.gif");

            Assert.AreEqual(true, !string.IsNullOrWhiteSpace(thumbnail));
        }

        [TestMethod]
        public void CreateThumbnailTest_ShouldPass_63x423()
        {
            HttpContext.Current = _context;

            var imageUrl = "http://www.bne.com.br/img/logo_bne.gif";

            var thumbnail = new ImageSlicer() { ThumbDir = "/thumbs/", ImageUrl = imageUrl }.CreateThumbnail(63, 423, 0, 480, 12, 12, "http://www.bne.com.br/img/logo_bne.gif");

            Assert.AreEqual(true, !string.IsNullOrWhiteSpace(thumbnail));
        }

    }
}
