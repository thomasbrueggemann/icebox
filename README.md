# ❄️ Icebox

Freezes contracual classes into snapshots for change detection

## Installation

```
PM> Install-Package Icebox
```

## Usage

First annotate your contractural classes with the `[Frozen]` attribute in order for `Icebox` to consider those classes.

```csharp
using System;
using Icebox.Attributes;

[Frozen]
public class ExampleApiResponseModel
{
	public int StatusCode { get; }
	public string Name { get; }
}
```

These annotations enable you to write a unit test that checks these frozen classes for changes.
It does so by writing a snapshot (we call that the _icebox_) of all the public properties of the frozen contracts to disk.
Everytime the unit test runs, it compares the current code base against the icebox snapshots.

```csharp
using System;
using System.Reflection;
using Xunit;
using AutoFixture.Xunit2;
using FluentAssertions;
using Icebox.Exceptions;

// ...

[Theory, AutoData]
public void ShouldCheckFrozenContracts(Icebox icebox)
{
	var assembly = Assembly.GetAssembly(typeof(ExampleApiResponseModel));
	Action act = () => icebox.Check(assembly);

	act.Should().NotThrow<FrozenContractException>();
}

// ...
```

Let's say over time we remove the `StatusCode` property on the `ExampleApiResponseModel` class

```csharp
using System;
using Icebox.Attributes;

[Frozen]
public class ExampleApiResponseModel
{
	public string Name { get; }
}
```

and run the unit test `ShouldCheckFrozenContracts`.
Now the assertion that no FrozenContractException trips and we have to restore the class to match the frozen contracts.

### Storing the .icebox files

bla

### What if I deliberately want to make breaking changes?

bla
