using System;
using System.Collections.Generic;
using System.Linq;
using Avelango.Models.Abstractions.Db;
using Avelango.Models.Abstractions.UnitOfWork;
using Avelango.Models.Accessory;
using Avelango.Models.Application;
using Avelango.Models.Orm;

namespace Avelango.DbOrm.Implementation
{
    public class ImpTaskAttachments : ITaskAttachments
    {
        private readonly IRepository<Tasks> _tasks;
        private readonly IRepository<TaskAttachments> _taskAttachments;


        public ImpTaskAttachments(IRepository<Tasks> tasks, IRepository<TaskAttachments> taskAttachments)
        {
            _tasks = tasks;
            _taskAttachments = taskAttachments;
        }


        public OperationResult<bool> Add(List<ApplicationTaskAttachment> files, Guid taskPk) {
            try {
                var task = _tasks.GetSingleOrDefault(x => x.PublicKey == taskPk);
                if (task == null) throw new Exception("[ImpTaskAttachments]/[Add]: Task does not found");
                foreach (var file in files) {
                    _taskAttachments.Add(new TaskAttachments {
                        BelongsToTask = task.ID,
                        Extention = file.Extention,
                        FileTitle = file.FileTitle,
                        PublicKey = file.PublicKey,
                        Url = file.Url
                    });
                }
                _taskAttachments.UnitOfWork.Commit();
                return new OperationResult<bool>();
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }



        public OperationResult<bool> Remove(List<Guid> removedfiles, Guid taskPk) {
            try {
                var task = _tasks.GetSingleOrDefault(x => x.PublicKey == taskPk);
                if (task == null) throw new Exception("[ImpTaskAttachments]/[Add]: Task does not found");
                var isRemoved = false;
                foreach (var i4Removed in removedfiles.Select(removedfile => 
                _taskAttachments.GetAll(x => x.BelongsToTask == task.ID && x.Url.Contains(removedfile.ToString()))).SelectMany(removed => {
                    var taskAttachmentses = removed as TaskAttachments[] ?? removed.ToArray();
                    return taskAttachmentses; })) {
                    _taskAttachments.Remove(i4Removed);
                    isRemoved = true;
                }
                if (isRemoved) _taskAttachments.UnitOfWork.Commit();
                return new OperationResult<bool>();
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }

    }
}
