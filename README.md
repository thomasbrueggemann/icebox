# ❄️ Icebox

Freezes contracual classes into snapshots for change detection

## Installation

```
PM> Install-Package Icebox
```

## Usage

First annotate your contractural classes with the `[Iceboxed]` attribute in order for `Icebox` to consider those classes.

```csharp
using System;
using Icebox.Attributes;

[Iceboxed]
public class ExampleApiResponseModel
{
	public int StatusCode { get; }
	public string Name { get; }
}
```

These annotations enable you to write a unit test that checks these iceboxed classes for changes.
It does so by writing a snapshot (we call that the _icebox_) of all the public properties of the iceboxed contracts to disk.
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
public void ShouldCheckIceboxedContracts(Icebox icebox)
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

[Iceboxed]
public class ExampleApiResponseModel
{
	public string Name { get; }
}
```

and run the unit test `ShouldCheckIceboxedContracts`.
Now the assertion that no IceboxedContractException trips and we have to restore the class to match the iceboxed contracts.

### Storing the .icebox files

bla

### What if I deliberately want to make breaking changes?

In order to deliberately make breaking changes, you have to delete the .icebox file(s), make your changes and then run the unit test that does the _Icebox check_ once again, to recreate a new baseline snapshot of the iceboxed contracts with your new changes.
From that moment on, the new contract will be frozen inside the Icebox once more.
