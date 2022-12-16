using _8obcy.Hubs;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;

namespace _8obcy
{
    public class GroupManager : IGroupManager
    {
        private IList<Group> Groups = new List<Group>();
        private int IdIncrementer = 0;
        public Group ChangeGroup(string connectionId)
        {
            RemoveGroupByConnectionId(connectionId);
            var destinationGroup = Groups.FirstOrDefault(x => x.Open);
            if (destinationGroup == null)
            {
                destinationGroup = GenerateNewGroup();
                Groups.Add(destinationGroup);
            }
            destinationGroup.ConnectionIds.Add(connectionId);
            if (destinationGroup.ConnectionIds.Count == 2)
            {
                destinationGroup.Open = false;
            }
            return destinationGroup;
        }
        public Group GetCurrentGroup(string connectionId)
        {
            return Groups.FirstOrDefault(x => x.ConnectionIds.Any(y => y == connectionId));
        }
        public void RemoveGroupByConnectionId(string connectionId)
        {
            var originGroup = Groups.FirstOrDefault(x => x.ConnectionIds.Any(y => y == connectionId));
            if (originGroup != null)
            {
                Groups.Remove(originGroup);
            }
        }
        private Group GenerateNewGroup()
        {
            IdIncrementer++;
            if (IdIncrementer == Int32.MaxValue)
            {
                IdIncrementer = 1;
            }
            return new Group
            {
                Id = IdIncrementer.ToString(),
                Open = true,
            };
            
            
        }
     
    }
}
