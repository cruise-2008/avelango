using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using Avelango.Handlers.Lang;
using Avelango.Models.Accessory;
using Avelango.Models.Application;
using Avelango.Models.Enums;

namespace Avelango.Handlers.File
{
    public static class FileManager
    {
        public static bool SaveFile(HttpPostedFileBase file, string path) {
            try {
                using (var fileStream = System.IO.File.Create(path)) {
                    file.InputStream.Seek(0, SeekOrigin.Begin);
                    file.InputStream.CopyTo(fileStream);
                }
                return true;
            }
            catch {
                return false;
            }
        }


        public static bool RemoveFile(string path) {
            try {
                System.IO.File.Delete(path);
                return true;
            }
            catch {
                return false;
            }
        }


        public static OperationResult<List<ApplicationTaskAttachment>> SaveTaskAttachments(Langs.LangsEnum currentLang, string root, List<HttpPostedFileBase> files) {
            var result = new OperationResult<List<ApplicationTaskAttachment>> {
                Data = new List<ApplicationTaskAttachment>()
            };
            try {
                var disabledExtentions = new List<string> { ".zip", ".rar", ".gzip", ".tar", ".arj", ".uc2", ".gz", ".lha", ".tgz", ".exe", ".bat", ".com" };
                var totalLength = files.Sum(t => t.InputStream.Length);
                if (totalLength >= 10 * 1024 * 1024) throw new Exception(ErrorManeger.GetErrorByName(currentLang, "FileLengthIsExceed"));
                foreach (var file in files) {
                    var ext = Path.GetExtension(file.FileName);
                    if (disabledExtentions.Contains(ext)) result.Exception = new Exception("ForbiddenFileExtention");
                    else {
                        var pk = Guid.NewGuid() + ext;
                        var path = @"\Storage\TaskAttachments\" + pk;
                        if (SaveFile(file, root + path)) {
                            result.Data.Add(new ApplicationTaskAttachment {
                                Extention = Path.GetExtension(file.FileName),
                                FileTitle = file.FileName,
                                PublicKey = Guid.NewGuid(),
                                Url = @".." + path
                            });
                        }
                    }
                }
                return result;
            }
            catch (Exception ex) {
                return new OperationResult<List<ApplicationTaskAttachment>>(ex);
            }
        }


        public static OperationResult<List<Guid>> RemoveTaskAttachments(List<string> filesPathes) {
            var removedAttacments = new OperationResult<List<Guid>> {
                Data = new List<Guid>()
            };
            try {
                foreach (var filePath in filesPathes) {
                    RemoveFile(filePath);
                    var fileMatch = Regex.Match(filePath, @"[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}");
                    if (string.IsNullOrEmpty(fileMatch.Value)) continue;
                    Guid fileNameGuid;
                    Guid.TryParse(fileMatch.Value, out fileNameGuid);
                    if (fileNameGuid != Guid.Empty) {
                        removedAttacments.Data.Add(fileNameGuid);
                    }
                }
                return removedAttacments;
            }
            catch (Exception ex) {
                return new OperationResult<List<Guid>>(ex);
            }
        }
    }
}
