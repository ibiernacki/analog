namespace Models.Settings
{
    public class Settings
    {
        private Settings(ParserType parserType)
        {
            ParserType = parserType;
        }

        public ParserType ParserType { get; }

        public static Builder NewBuilder()
        {
            return new Builder();
        }

        public Builder GetBuilder() { return new Builder(this); }

        public class Builder
        {
            private ParserType _parserType;

            public Builder()
            {

            }

            public Builder(Settings settings)
            {
                _parserType = settings.ParserType;
            }

            public Builder ParserType(ParserType parserType)
            {
                _parserType = parserType;
                return this;
            }

            public Settings Build()
            {
                return new Settings(_parserType);
            }
        }
    }
}
