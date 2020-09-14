namespace DRON.Parse
{
    public class DronNumber : DronNode
    {
        #region Public

        #region Constructors
        public DronNumber(double value)
        {
            Value = value;
        }
        #endregion

        #region Members
        public readonly double Value;
        #endregion

        #endregion
    }
}