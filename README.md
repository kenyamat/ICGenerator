# InterfaceContractsGenerator
InterfaceContractsGenerator is a library for generating Interface Code Contracts template files from interface source code.

[Microsoft CodeContracts](http://research.microsoft.com/en-us/projects/contracts/) are static library methods used from any .NET program to specify the code’s behavior. Runtime checking and static checking tools are both provided for taking advantage of contracts.

Interface Code Contracts help interface layer of three-layer application to state preconditions and postconditions of method. But it requires a source code for each interface.

This library supports the creation of contracts source file templates.
## Usage

```
InterfaceContractsGenerator.exe -i [inputDirectoryPath] -o [ouputDirectoryPath]
```

### sample command


```
InterfaceContractsGenerator.exe -i "C:\GitProjects\Battle\Battle.Domain" -o "C:\Temp\Output"
```

## Limitations
* This library just appends `Contract.Requires<System.ArgumentNullException>` for parameters and `Contract.Ensure` for return values. Please add more detailed contracts If you need.
* Multiple inheritance are not supported. Please modify generated codes.

## Sample Output

### Input
```

using System;

namespace TestSpace
{
    /// <summary>
    /// ITest interface
    /// </summary>
    public interface ITest
    {
        /// <summary>
        /// Indexer
        /// </summary>
        /// <param name="i">index</param>
        /// <returns>value</returns>
        string this[string i] { get; set; }

        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Creates something.
        /// </summary>
        /// <param name="name">the name</param>
        /// <returns>something</returns>
        string Create(string name);
    }
}
```

### Output

```
using System.Diagnostics.Contracts;

namespace TestSpace.Contracts
{
    /// <summary>
    /// ITest interface
    /// </summary>
    [ContractClassFor(typeof(ITest))]
    internal abstract class TestContracts : ITest
    {
        /// <summary>
        /// Indexer
        /// </summary>
        /// <param name="i">index</param>
        /// <returns>value</returns>
        public string this[string i]
        {
            get
            {
                Contract.Requires<System.ArgumentNullException>(i != null, "'i' must not be null.");
                return default(string);
            }
            set
            {
                Contract.Requires<System.ArgumentNullException>(i != null, "'i' must not be null.");
                Contract.Requires<System.ArgumentNullException>(value != null, "'value' must not be null.");
            }
        }

        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string Name
        {
            get { return default(string); }
            set {}
        }

        /// <summary>
        /// Creates something.
        /// </summary>
        /// <param name="name">the name</param>
        /// <returns>something</returns>
        public string Create(string name)
        {
            Contract.Requires<System.ArgumentNullException>(name != null, "'name' must not be null.");
            Contract.Ensures(Contract.Result<string>() != null, "The return value must not be null.");
            return default(string);
        }
    }
}
```

## Copyright
Copyright (c) 2014 kenyamat Licensed under MIT. See [LICENSE] for details.