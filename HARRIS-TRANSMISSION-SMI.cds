I-- HARRIS-TRANSMISSION-SMI:  Harris Broadcast Transmission System: Structure of Management Information
--
-- Copyright 2001-2011 Harris Corporation.  All Rights
-- Reserved.  Reproduction of this document is authorized on
-- condition that the foregoing copyright notice is included.
-- This Harris SNMP Management Information Base Specification
-- (Specification) embodies Harris' confidential and
-- proprietary intellectual property.  Harris retains all
-- title and ownership in the Specification, including any
-- revisions.
-- Revision history
-- see MODULE IDENTITY
                                     U"The structure of management information (SMI) for
		Harris' Broadcast System MIBs." g"Harris Corporation
		Broadcast Systems
		5300 Kings Island Drive, Suite 101
		Mason, OH 45040, USA" "1104200000Z" "1012070000Z" "1011180000Z" "1002050000Z" "1001220000Z" "1001200000Z" "0911110000Z" "0911100000Z" "0910200000Z" "0604100000Z" "0601230000Z" "0601180000Z" "0509260000Z" "0507150000Z" "0503250000Z" "0512170000Z" "0411030000Z" "0410270000Z" "0408260000Z" "0302040000Z" "0301200000Z" "0211210000Z" "0110300900Z" @"Added FAX TX and NPlus1 Branch.
	   Zhiqun Hu, zhu@harris.com" 8"Added micromax exciter.
	   Zhiqun Hu, zhu@harris.com" O"Added zxa TX and moved HPX to transmitters 30.
	   Zhiqun Hu, zhu@harris.com" P"Define uax,vax,uaxST,uaxDD,vaxST,vaxDD.
	   Vincent Li, vincent.li@harris.com" ]"Define FLO and other modulation nodes under exciter.
	   Vincent Li, vincent.li@harris.com" I"Added base and modulation under exciter.
	   Zhiqun Hu, zhu@harris.com" :"Added vlx, uax, hpx, zxb.
	   Zhiqun Hu, zhu@harris.com" -"Added ulx.
	   Carrie Li, cli01@harris.com" /"Added lband.
	   Carrie Li, cli01@harris.com" 9"Added apexD2.
	   Shilpa Amarnath, samarnat@harris.com" ="Added platinumA3.
	   Shilpa Amarnath, samarnat@harris.com" :"Added atlasD2.
	   Shilpa Amarnath, samarnat@harris.com" :"Added 3dxhpA1.
	   Shilpa Amarnath, samarnat@harris.com" C"Added custspecial branch.
	   Manfred Schmid, mschmid@harris.com" ;"Added platinumD3.
	   Manfred Schmid, mschmid@harris.com" ;"Added platinumA2.
	   Manfred Schmid, mschmid@harris.com" O"Assigned atlasA1 OID 17 instead of 8.
	   Manfred Schmid, mschmid@harris.com" <"Added gpio branch.
	   Manfred Schmid, mschmid@harris.com" D"Added powerCD transmitter.
	   Manfred Schmid, mschmid@harris.com" <"Official revision.
	   Manfred Schmid, mschmid@harris.com" �"Added excitersB. Renamed exciters to excitersA. Assigned supervisors OID 4 instead of OID 3. Added device list.
	   Manfred Schmid, mschmid@harris.com" q"Put transmission into the right branch (HARRIS-MIB). Added supervisors.
	   Manfred Schmid, mschmid@harris.com" S"Initial revision.
	   Mike Suchoff, In Tune, Inc. mikes@in-tune.com 919-933-8495"       --20Apr2011
       ,"RF transmitters are defined in this branch"                "uax are defined in this branch"                "vax are defined in this branch"               *"RF exciters A are defined in this branch"               *"RF exciters B are defined in this branch"               |"supervisors are defined in this branch
         supervisors are devices controlling or monitoring groups of other devices"               &"receivers are defined in this branch"               I"general purpose IO used for external control are defined in this branch"               $"exciter are defined in this branch"               ."TX Nplus1 systems are defined in this branch"               �"The root OID from which customer specials start. Each customer special shall be given a unique
         OID in this branch by assigning a consecutive number starting at 1."               �"The root OID from which experimental and/or work-in-progress mibs
        are developed.  These OIDs are temporary and are removed when permanent
        OIDs (usually within products) are assigned."               D"The root OID from which AGENT-CAPABILITIES values may be assigned."               C"The root OID from which MODULE-COMPLIANCE values may be assigned."                  