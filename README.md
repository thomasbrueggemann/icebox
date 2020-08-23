# ❄️ Icebox

Freezes contracual classes into snapshots for change detection in integration tests

## Installation

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
