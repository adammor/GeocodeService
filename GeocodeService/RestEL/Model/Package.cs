using System.Collections.Generic;

namespace RestEL.Models
{

public class Package
{
public Settings settings { get; set; }
}

public class Settings
{
public Generalsettingsselected generalSettingsSelected { get; set; }
public Apiselected APISelected { get; set; }
public Repositoryselected repositorySelected { get; set; }
}

public class Generalsettingsselected
{
public List<Runoption> runOptions { get; set; }
}

public class Runoption
{
public Runoption() { roundRobin = "Fixed"; }
public string roundRobin { get; set; }
}

public class Apiselected
{
public string type { get; set; }
public string APIName { get; set; }
public string planType { get; set; }
}

public class Repositoryselected
{
public string Source { get; set; }
public string Target { get; set; }
}

}


