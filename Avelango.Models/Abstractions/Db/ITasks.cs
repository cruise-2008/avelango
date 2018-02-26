using System;
using System.Collections.Generic;
using Avelango.Models.Accessory;
using Avelango.Models.Application;

namespace Avelango.Models.Abstractions.Db
{
    public interface ITasks
    {
        OperationResult<IEnumerable<ApplicationTask>> GetFilteredTasks(Guid userPk, List<Guid> viewedTasks, List<string> subgroups, bool justActual, int n, double? placeLat, double? placeLng);
        OperationResult<ApplicationTask> GetTaskByPk(Guid userPk, Guid taskPk);
        OperationResult<List<ApplicationTask>> GetTasksByPks(Guid userPk, List<Guid> taskPk);

        OperationResult<Guid> AddTasks(string name, string description, string price, string placeName, double? placeLat, double? placeLng, DateTime topicalto, string userid);
        OperationResult<ApplicationTask> ConfirmTask(Guid id, string group, string subgroup);

        OperationResult<IEnumerable<ApplicationTask>> GetMyOrders(Guid userId);
        OperationResult<IEnumerable<ApplicationTaskForWorker>> GetMyJobs(Guid userId);


        // Statuses changes
        OperationResult<bool> MyOrderSetBiderAsExecutor(Guid executorId, Guid taskPk);
        OperationResult<bool> MyOrderCheckIsEditable(Guid taskPk);
        OperationResult<List<ApplicationUser>> MyOrderGetBiders(Guid userPk, Guid taskPk);
        OperationResult<bool> MyOrderEdit(Guid userPk, Guid taskPk, string title, string discription, int price, string placeName, double? placeLat, double? placeLng, DateTime topicalto);
        OperationResult<ApplicationTask> MyOrderRemove(Guid userPk, Guid taskPk);
        OperationResult<bool> MyOrderReopen(Guid userPk, Guid taskPk);
        OperationResult<ApplicationTask> MyOrderClose(Guid userPk, Guid taskPk);
        OperationResult<bool> MyOrderToDisput(Guid userPk, Guid taskPk);

        OperationResult<ApplicationTask> TaskStartWorking(Guid userPk, Guid taskPk);


        OperationResult<IEnumerable<ApplicationTask>> GetTasksAwaitedModerators();
        OperationResult<ApplicationTask> TaskChecked(Guid pk, string name, string description, string group, string subGroup, int price, bool success);

        OperationResult<object> CountIncompleteTasks(Guid userPk);
        OperationResult<object> CountIncompleteJobs(Guid userPk);
    }
}
