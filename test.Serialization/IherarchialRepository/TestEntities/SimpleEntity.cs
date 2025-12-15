namespace test.Serialization
{
        class SimpleEntity
        {
           public int a;

            public SimpleEntity()
            {
            }

            public SimpleEntity(int a)
            {
                this.a = a;
            }
        public override bool Equals(object obj)
        {
            return (obj is SimpleEntity)? (obj as SimpleEntity).a == a:false;
        }
    }

    }

