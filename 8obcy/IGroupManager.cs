namespace _8obcy
{
    public interface IGroupManager
    {
        public string ChangeGroup(string connectionId);
        public string GetCurrentGroup(string connectionId);
        public void RemoveGroup(string connectionId);
        
    }
}
