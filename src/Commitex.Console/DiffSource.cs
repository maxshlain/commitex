namespace Commitex.Console;

public class DiffSource
{
    public string Source {
        get
        {
            return @"
            diff --git a/Commitex.Console/Commitex.Console/appsettings.json b/Commitex.Console/Commitex.Console/appsettings.json
            index 1677ced..81f252e 100644
                    --- a/Commitex.Console/Commitex.Console/appsettings.json
                    +++ b/Commitex.Console/Commitex.Console/appsettings.json
                @@ -7,5 +7,6 @@
        }
    },
     
    -    ""Token"" : """"
    +    ""Token"" : """",
    +    ""Enabled"" : true
}
";
        }
    }
}