@echo off

title FileService

cd /d %~dp0

set ASPNETCORE_URLS=http://*:5001
set ASPNETCORE_ENVIRONMENT=Production

Mondol.FileService.Web.exe