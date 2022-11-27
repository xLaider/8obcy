using _8obcy.Hubs;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;

namespace _8obcy
{
    public class GroupManager : IGroupManager
    {
        private IList<Group> Groups = new List<Group>();
        private static Random rnd = new Random();
        public string ChangeGroup(string connectionId)
        {

            var originGroup = Groups.FirstOrDefault(x => x.ConnectionIds.Any(y => y == connectionId));
            if (originGroup != null)
            {
                originGroup.ConnectionIds.Remove(connectionId);
            }
            var possibleGroups = Groups.Where(x => x.Open).ToList();
            if (Groups.Count == 0 || possibleGroups.Count == 0)
            {
                GenerateNewGroup();
                possibleGroups.Add(Groups.Last());
            }
            var destinationGroup = possibleGroups[rnd.Next(possibleGroups.Count)];
            destinationGroup.ConnectionIds.Add(connectionId);
            if (destinationGroup.ConnectionIds.Count == 2)
            {
                destinationGroup.Open = false;
            }
            if (originGroup != null)
            {
                if (originGroup.ConnectionIds.Count == 0)
                {
                    Groups.Remove(originGroup);
                }
                else
                {
                    originGroup.Open = true;
                }
            }
            return destinationGroup.Id.ToString();


        }

        public string GetCurrentGroup(string connectionId)
        {
            return Groups.FirstOrDefault(x => x.ConnectionIds.Any(y => y == connectionId)).Id.ToString();
        }

        private void GenerateNewGroup()
        {
            Groups.Add(new Group
            {
                Id = Groups.Count == 0 ? 1 : Groups.Count + 1,
                Open = true,
            });
        }
    }
}
