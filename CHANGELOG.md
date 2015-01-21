

## vNext

* Allow period (`.`) in Msisdn input.
* Add more specific Country Code descriptions in Zone 1 (North America).
* Add length validation (pr country) ?

## 0.3.0

* Use a precompiled regex for Msisdn validation.
* Restrict Country Code to three digits.
* Accept 90533 and 90542 (in addition to 90392) as prefixes for Northern Cyprus, not Turkey. International country code will be 90.

## 0.2.0

* Require qualified access to module content (no longer possible to `open Msisdn`).
* Validate that Msisdn is at least 8 digits (not part of the standard, but sensible minimum).

## 0.1.0

Initial version