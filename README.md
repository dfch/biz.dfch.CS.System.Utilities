# biz.dfch.CS.Utilities
[![Build Status](https://build.dfch.biz/app/rest/builds/buildType:(id:CSharpDotNet_BizDfchCsSystemUtilitiesGit_Build)/statusIcon)](https://build.dfch.biz/project.html?projectId=CSharpDotNet_BizDfchCsSystemUtilitiesGit&tab=projectOverview)
[![License](https://img.shields.io/badge/license-Apache%20License%202.0-blue.svg)](https://github.com/dfch/biz.dfch.CS.System.Utilities/blob/master/LICENSE)

Assembly: biz.dfch.CS.Utilities.dll

d-fens GmbH, General-Guisan-Strasse 6, CH-6300 Zug, Switzerland

## Download

* Get it on [NuGet](https://www.nuget.org/packages/biz.dfch.CS.System.Utilities/)

* See [Releases](https://github.com/dfch/biz.dfch.CS.System.Utilities/releases) on [GitHub](https://github.com/dfch/biz.dfch.CS.System.Utilities)

## Description

This project containts a collection of utility classes that provide functionalities like:

* Invocation of RESTful requests (GET, HEAD, POST, PUT, DELETE)
* StringValueAttribute for Enums
* Cryptography helper for password encryption and decryption
* Start-Process assembly for invocation of processes via Interop/CreateProcessWithLogonW for use with biz.dfch.PS.System.Utilities
  See http://d-fens.ch/2014/10/11/bug-start-job-from-scheduled-task-fails-with-event-id-8197-when-invoked-via-different-credential-set/
* Log forwarders for System.Diagnostics.Debug/Trace for log4net
* HttpApplication events for MEF plugins
* ODataEndpoint interfaces for MEF plugins
* Json helper methods for NewtonSoft.Json
* Introspection and Reflection for calling methods
* Http message response helpers and exceptions

**Telerik JustMock has to be licensed separately. Only the code samples (source code files) are licensed under the Apache 2.0 license. The Telerik JustMock software has to be licensed separately. See the NOTICE file for more information about this.**

## [Release Notes](https://github.com/dfch/biz.dfch.CS.System.Utilities/releases)

See also [Releases](https://github.com/dfch/biz.dfch.CS.System.Utilities/releases) and [Tags](https://github.com/dfch/biz.dfch.CS.System.Utilities/tags)

### 1.2.7 - 20150926

* ignore CodeContracts test as it fails on TeamCity
* make setter on LogBase.log (log4net.ILog) private to better support testing

### 1.2.6 - 20150926

* add attribute to test Code Contract exceptions
* add more Json tests
* add MemberwiseClone to BaseEntity
* add HttpStatusException and WebApi filter
* add DbEntityValidationException WebApi filter
* add EntityFramework 6.1.3 dependency
* add ODataExceptionFilterAttribute WebApi filter
* add CatchAll WebApi filter
* add OData GetLinkFromUri helper
* enable code contract runtime checks on ~.Tests project
* added test for CodeContract exception attribute
* add log4net assembly directive to load configuration from separate xml file
* make setter on LogBase.log (log4net.ILog) private to better support testing

### 1.2.5 - 20150910

* BaseEntity extended

### 1.2.4

* ODataControllerHelper added

### 1.2.3

* BaseEntity added
* Missing changes in csproj file added

### 1.2.2

* BaseEntity added

### 1.2.1

* bugfix for failing TeamCity build

### 1.2.0

* Contracts for ODataEndpoints with version support note this is a breaking interface change!
* HttpApplication events
* Json helpers
* Logging helpers
* Http message helpers
* Introspection for calling methods

### 1.1.0

* Contracts for ODataEndpoints added
* HttpRequestMessage extensions added

### 1.0.4

* NuGet package creation adjusted to only deliver dll in package

### 1.0.3

* Namespace adjusted to avoid namespace overlap

### 1.0.2

* Assembly name adjusted

### 1.0.1

* Maintenance release

### 1.0.0

* Initial release
