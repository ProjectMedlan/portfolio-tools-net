# Portfolio Performance Tools
Read the data from your Portfolio Performance file and use it in your .NET Application.

## Install Instructions
Currently nothing to install.

## Roadmap
 * Read further information from the file (there are plenty of possibilities). Check if there's something useful. (Added some TODOs to code)
 * Check: Deserialize XML to complete object - benefits?

## Changelog
 * 04.01.2026: Update: .NET 10
 * 09.04.2022: Fixed: Removed sold securities from report (total shares = 0)
 * 10.02.2022: Added: Sample Application
 * 06.02.2022: Added: DivvyDiaryService: Read latest dividens from DivvyDiaray
 * 30.01.2022: Fixed: Total shares have not been reduced for type sell only for type delivery outbound
 * 30.01.2022: Added: Setting to skip securitys with no shares when exporting to csv
 * 23.01.2022: Added: First version of csv export (with test)
 * 22.01.2022: Added: Read the buy / sell data from the Portfolio Performance file
 * 15.01.2022: Added: Read the securities from the Portfolio Performance file
 * 15.01.2022: Added: First unit tests
