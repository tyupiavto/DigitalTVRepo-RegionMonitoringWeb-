�-- HARRIS BASE TRANSMITTER MIB
-- Copyright 2002-2005 Harris Corporation.  All Rights
-- Reserved.  Reproduction of this document is authorized on
-- condition that the foregoing copyright notice is included.
-- This Harris SNMP Management Information Base Specification
-- (Specification) embodies Harris' confidential and
-- proprietary intellectual property.  Harris retains all
-- title and ownership in the Specification, including any
-- revisions.
-- Compatibility policy:
-- If the initial revision date is the same, then 2 versions of a MIB are compatible
-- If the initial revision date differs, then the version with the newer date invalidates all older versions.
   "switch exciter"               "switch exciter"               "switch exciter"               a"basic control commands
                 0        reserved
                 600..699 reserved "               "Assigned to status"                                                 -"MIB for all common data of the transmitters"O"Harris Corporation
                                          Vincent Li
                                          Broadcast Systems
                                          5300 Kings Island Drive, Suite 101
                                          Mason, OH 45040, USA
                                          zhu@harris.com" "1011260000Z" "1008120000Z" "1002100000Z" "0502150000Z" "0309080000Z" �"Change allTrapEnable from TruthValue into Integer32
                                          to extend its range.
	                                        Vincent Li, vincent.li@harris.com" �"add drvControl branch, excSwitch,excSwitchMode,auxControl,individual trap enable
	                                        Vincent Li, vincent.li@harris.com" _"add allTrapEnable
	                                        Vincent Li, vincent.li@harris.com" K"added outAuralForwardPower, outAuralReflectedPower, outVSWR, outAuralVSWR" "Initial revision"       --26Nov2010
           �"    Connection ecdi to transmitter: 1=Fault, 2=Ok


                                                                                                             "                 �"    Drive chain summary fault: 1=Fault, 2=Ok


                                                                                                             "                 �"    Drive chain summary warning: 1=Fault, 2=Ok


                                                                                                             "                 �"    Inputsignal ok: 1=Ok, 2=Not ok


                                                                                                             "                 �"    Power amplifier summary fault: 1=Fault, 2=Ok


                                                                                                             "                 �"    Power amplifier summary warning: 1=Warning, 2=Ok


                                                                                                             "                 �"    Power supply summary fault: 1=Fault, 2=Ok


                                                                                                             "                 �"    Power supply summary warning: 1=Warning, 2=Ok


                                                                                                             "                 �"    Output summary fault: 1=Fault, 2=Ok


                                                                                                             "                 �"    Output summary warning: 1=Warning, 2=Ok


                                                                                                             "                 �"    RF output signal ok: 1=Ok, 2=Not ok


                                                                                                             "                 �"    System section summary fault: 1=Fault, 2=Ok


                                                                                                             "                 �"    System section summary warning: 1=Warning, 2=Ok


                                                                                                             "                 �"    Transmitter overall summary fault: 1=Fault, 2=Ok


                                                                                                             "                 �"    Transmitter overall summary warning: 1=Warning, 2=Ok


                                                                                                             "                 �"    Remote Enable: 1=Remote, 2=Local


                                                                                                             "                 �"    Trap generated during configuration to test the trap connectivity


                                                                                                             "                         �"       Drive chain summary fault: 1=Fault, 2=Ok

                                                                                                             "                       �"       Drive chain summary warning: 1=Fault, 2=Ok


                                                                                                             "                           �"       Inputsignal ok: 1=Ok, 2=Not ok


                                                                                                             "                           �"       exciter Switch: 1=ExciterA On, 2=ExciterB On


                                                                                                             "                       �"       exciter Switch Mode: 1=auto, 2=manual


                                                                                                             "                               �"       Power amplifier summary fault: 1=Fault, 2=Ok

                                                                                                             "                       �"       Power amplifier summary warning: 1=Warning, 2=Ok

                                                                                                             "                               �"       Power supply summary fault: 1=Fault, 2=Ok


                                                                                                             "                       �"       Power supply summary warning: 1=Warning, 2=Ok


                                                                                                             "                               �"       Output summary fault: 1=Fault, 2=Ok

                                                                                                             "                       �"       Output summary warning: 1=Warning, 2=Ok


                                                                                                             "                           �"       RF output signal ok: 1=Ok, 2=Not ok


                                                                                                             "                           �"       System forward power level. 


                                                                                                             "                       �"       System reflected power level

                                                                                                             "                       �"       Factor outReflectedPower/outForwardPower. Fr = 100 * log
                                (outReflectedPower/outForwardPower)
                                                                                                             "                       �"       System VSWR


                                                                                                             "                       �"       System aural forward power level (valid for analog Tx only)


                                                                                                             "                       �"       System aural reflected power level (valid for analog Tx only)

                                                                                                             "                       �"       Aural VSWR (valid for analog Tx only)


                                                                                                             "                               �"       Connection ecdi to transmitter: 1=Fault, 2=Ok


                                                                                                             "                       �"       System section summary fault: 1=Fault, 2=Ok


                                                                                                             "                       �"       System section summary warning: 1=Warning, 2=Ok

                                                                                                             "                       �"       Transmitter overall summary fault: 1=Fault, 2=Ok


                                                                                                             "                       �"       Transmitter overall summary warning: 1=Warning, 2=Ok


                                                                                                             "                       �"       Trap priority level: 1=(Sum)Fault, 2=(Sum)Warning, 3=Info

                                                                                                             "                       �"       Trap details

                                                                                                             "                       �"       Running index for events


                                                                                                             "                           �"       Remote Enable: 1=Remote, 2=Local


                                                                                                             "                       �"       Transmitter on state enumerations: 0=NA, 1=On, 2=Off

                                                                                                             "                       �"       Trap generated during configuration to test the trap connectivity


                                                                                                             "                           �"       Transmitter control enumerations: 1=On, 2=Off, 3=RaisePwr,
                                4=LowerPwr
                                                                                                             "                       �"       Argument for txControl

                                                                                                             "                       �"    AuxControlValue: 1 = auxOn(Customer Sepcial), 2 = auxOff(Customer Sepcial)

                                                                                                             "                       �"    Enable/Disable all traps. 0= Enable/Disble trap individually, 1=Enable all traps, 2=Disable all traps.

                                                                                                             "                       �"    Enable/Disable dataSourceOfflineTrap: 1 = Enable trap, 2 = Disable trap

                                                                                                             "                       �"    Enable/Disable drvInputOkTrap: 1 = Enable trap, 2 = Disable trap

                                                                                                             "                       �"    Enable/Disable drvSumFaultTrap: 1 = Enable trap, 2 = Disable trap

                                                                                                             "                       �"    Enable/Disable drvSumWarningTrap: 1 = Enable trap, 2 = Disable trap

                                                                                                             "                       �"    Enable/Disable outOkTrap: 1 = Enable trap, 2 = Disable trap

                                                                                                             "                       �"    Enable/Disable outSumFaultTrap: 1 = Enable trap, 2 = Disable trap

                                                                                                             "                       �"    Enable/Disable outSumWarningTrap: 1 = Enable trap, 2 = Disable trap

                                                                                                             "                       �"    Enable/Disable paSumFaultTrap: 1 = Enable trap, 2 = Disable trap

                                                                                                             "                       �"    Enable/Disable paSumWarningTrap: 1 = Enable trap, 2 = Disable trap

                                                                                                             "                       �"    Enable/Disable psSumFaultTrap: 1 = Enable trap, 2 = Disable trap

                                                                                                             "                       �"    Enable/Disable psSumWarningTrap: 1 = Enable trap, 2 = Disable trap

                                                                                                             "                       �"    Enable/Disable remoteEnableTrap: 1 = Enable trap, 2 = Disable trap

                                                                                                             "                       �"    Enable/Disable sysSumFaultTrap: 1 = Enable trap, 2 = Disable trap

                                                                                                             "                       �"    Enable/Disable sysSumWarningTrap: 1 = Enable trap, 2 = Disable trap

                                                                                                             "                       �"    Enable/Disable txSumFaultTrap: 1 = Enable trap, 2 = Disable trap

                                                                                                             "                       �"    Enable/Disable txSumWarningTrap: 1 = Enable trap, 2 = Disable trap

                                                                                                             "                           "       Title

                                                                                                             "                       �"       Model name (ShortName)


                                                                                                             "                       �"       Transmitter frequency


                                                                                                             "                           "base objects"                 "base events"                                