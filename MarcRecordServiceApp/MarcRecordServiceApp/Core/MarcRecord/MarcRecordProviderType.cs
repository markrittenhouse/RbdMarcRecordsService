namespace MarcRecordServiceApp.Core.MarcRecord
{
    public enum MarcRecordProviderType
    {        
        Lc = 1,    
        Nlm = 2,
        Rbd = 3
    }
    public class MarcRecordProviderValue
    {
        private readonly string _provider;
        public MarcRecordProviderValue(MarcRecordProviderType value)
        {
            switch (value)
            {
                  case MarcRecordProviderType.Lc:
                    _provider = "z3950.loc.gov";
                    break;
                  case MarcRecordProviderType.Nlm:
                    _provider = "ilsz3950.nlm.nih.gov";
                    break;
                  case MarcRecordProviderType.Rbd:
                    _provider = "";
                    break;
            }
        }
        public new string ToString()
        {
            return _provider;
        }
    }
}
