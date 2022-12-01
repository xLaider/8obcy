using _8obcy.Hubs;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;

namespace _8obcy
{
    public class GroupManager : IGroupManager
    {
        private IList<Group> Groups = new List<Group>();
        private int IdIncrementer = 0;
        public string ChangeGroup(string connectionId)
        {
            RemoveGroup(connectionId);
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
            return destinationGroup.Id.ToString();
        }
        public string GetCurrentGroup(string connectionId)
        {
            var currentGroup = Groups.FirstOrDefault(x => x.ConnectionIds.Any(y => y == connectionId));
            return currentGroup == null ? String.Empty : currentGroup.Id.ToString();
        }
        public void RemoveGroup(string connectionId)
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
            return new Group
            {
                Id = IdIncrementer,
                Open = true,
            };
            
        }
     
    }
}
