[CmdletBinding()]
Param(

    [Parameter(Mandatory=$True,Position=1)][string]$ServerName,
    [Parameter(Mandatory=$True,Position=2)][string]$WindowsServiceName,
    [Parameter(Mandatory=$True,Position=3)][string]$Action
)


Function StartStopWindowsService([string]$ServerName,[string]$WindowsServiceName,[string]$Action)
{
    try
    {
        $winService = Get-Service -ComputerName $ServerName -Name $WindowsServiceName
        If($winService)
        {
            #Write-Host "Starting $WindowsServiceName Service from $ServerName"
            if($Action -eq "Start")
            {
                if($winService.Status -eq "Stopped")
                {
                    $winService.Start()
                }
                $winService.WaitForStatus("Running")
                If($winService.Status -eq "Running")
                {
                    #Write-Host "$WindowsServiceName Service from $ServerName has started!"
					Write-Output $winService.Status
                }
                else
                {
					Write-Output  "Not able to start $WindowsServiceName"
                    #Write-Host "Not able to start $WindowsServiceName Junifer Windows service from $ServerName, please check/stop manually!"
                    Exit        
                }
            }
            elseif($Action -eq "Stop")
            {
                if($winService.Status -eq "Running")
                {
                    $winService.Stop()
                }
                $winService.WaitForStatus("Stopped")
                If($winService.Status -eq "Stopped")
                {
                    #Write-Host "$WindowsServiceName Service from $ServerName has stopped!"
					Write-Output $winService.Status
				}
                else
                {
                    Write-Host "Not able to stop $WindowsServiceName Junifer Windows service from $ServerName, please check/stop manually!"
                    Exit        

                }
            }
            elseif($Action -eq "Status")
            {
                Write-Output $winService.Status
            }         
        }
        else
        {
            Write-Output "No Services available on Name:$WindowsServiceName in Server:$ServerName"
            Exit
        }
    }
    catch
    {
        Write-Output $_.Exception.Message
        throw
        Exit
    }
}

#Write-Output "Parameters value"
#Write-Output $ServerName
#$WindowsServiceName

StartStopWindowsService -ServerName $ServerName -WindowsServiceName $WindowsServiceName -Action $Action
