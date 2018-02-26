using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Avelango.Handlers.Image
{
    public static class ImgHandler
    {
        public static System.Drawing.Image Crop(System.Drawing.Image image, Rectangle cropRect) {
            try {
                var src = image as Bitmap;
                var res = new Bitmap(cropRect.Width, cropRect.Height);
                using (var g = Graphics.FromImage(res)) {
                    if (src != null) {
                        g.DrawImage(src, new Rectangle(0, 0, res.Width, res.Height), cropRect, GraphicsUnit.Pixel);
                    }
                }
                return res;
            }
            catch {
                return null;
            }
        }



        public static System.Drawing.Image CreateMiniImage (System.Drawing.Image image, int width, int height) {
            try {
                var destRect = new Rectangle(0, 0, width, height);
                var destImage = new Bitmap(width, height);
                destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);
                using (var graphics = Graphics.FromImage(destImage)) {
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    using (var wrapMode = new ImageAttributes()) {
                        wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                        graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                    }
                }
                return destImage;
            }
            catch {
                return null;
            }
        }


        public static bool RemoveImage(string imagePath) {
            try {
                var exist = System.IO.File.Exists(imagePath);
                if (exist) System.IO.File.Delete(imagePath);
                return !System.IO.File.Exists(imagePath);
            }
            catch {
                return false;
            }
        }


        public static bool SaveImage(string base64ImageContent, string path) {
            try {
                var bytes = Convert.FromBase64String(base64ImageContent);
                var image = Base64ToImage(bytes);
                var newBitmap = new Bitmap(image);
                image.Dispose();
                newBitmap.Save(path, ImageFormat.Png);
                return true;
            }
            catch {
                return false;
            }
        }


        public static bool SaveImage(System.Drawing.Image image, string path) {
            try {
                using (var newBitmap = new Bitmap(image)) {
                    newBitmap.Save(path, ImageFormat.Png);
                }
                return true;
            }
            catch {
                return false;
            }
        }


        public static System.Drawing.Image Base64ToImage(string data) {
            try {
                var bytes = Convert.FromBase64String(data);
                return Base64ToImage(bytes);
            }
            catch {
                return null;
            }
        }


        public static System.Drawing.Image Base64ToImage(byte[] data) {
            try {
                System.Drawing.Image image;
                using (var ms = new MemoryStream(data)) {
                    image = System.Drawing.Image.FromStream(ms);
                }
                return image;
            }
            catch {
                return null;
            }
        }
    }
}
