### Nova

A simple expression evaluator/programming language. Written with the help of [Immo Landwerth](https://www.youtube.com/channel/UCaFP8iQMTuPXinXBMEXsSuw)

## Setting up locally

To set up the repository, first clone it:
```
git clone https://github.com/YeffyCodeGit/Nova
```

Then, go into the scripts directory and run the unit tests:
```
cd scripts
./Tests.ps1
```

On Windows, if you get an error, it might be because your Execution Policy is set to restricted. You can check by running:
```
Get-ExecutionPolicy
```

If it says restricted, open an Adminstrator Powershell window and run:
```
Set-ExecutionPolicy Unrestricted
```
