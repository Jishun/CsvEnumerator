# CsvEnumerator
A simple enumerator for CSV files, output it as a table like 2 dimension list for further analysing

## Installation 
* Nuget pacakge available at https://www.nuget.org/packages/CsvEnumerator.NetStandard/
* Older .netframework versions at https://www.nuget.org/packages/CsvEnumerator

## Example Usage:
```cs
var logs = new List<string>();
var str = new SeekableString(file);
var csv = str.ValidateCsv(logs, true, false);
if(logs.Count == 0){
	foreach(var line in csv){
		foreach(var cell in line){
			//do someting with the string list
		}
	}
}else{
	//csv is not valid
}
```
