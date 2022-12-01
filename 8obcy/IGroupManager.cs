namespace _8obcy
{
    public interface IGroupManager
    {
        public Group ChangeGroup(string connectionId);
        public Group GetCurrentGroup(string connectionId);
        public void RemoveGroupByConnectionId(string connectionId);
        
    }
}
