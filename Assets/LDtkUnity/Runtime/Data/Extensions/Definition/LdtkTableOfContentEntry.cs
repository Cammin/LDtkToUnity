namespace LDtkUnity
{
    public partial class LdtkTableOfContentEntry
    {
        //to correct for the legacy entries
        public LdtkTocInstanceData[] UnityInstances()
        {
            //new data or legacy instances
            if (InstancesData != null)
            {
                return InstancesData;
            }
            if (Instances != null)
            {
                LdtkTocInstanceData[] data = new LdtkTocInstanceData[Instances.Length];
                for (int i = 0; i < Instances.Length; i++)
                {
                    data[i] = new LdtkTocInstanceData() { Iids = Instances[i] };
                }
                return data;
            }
            return null;
        }
        
    }
}