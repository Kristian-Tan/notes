# Wrapping Progressive Web Application
- wrapping is needed to publish in application market that only accept native application (example: google play store, apple app store)
- wrapping creates android studio project / xcode project, that can later be built into apk

## Methods to Wrap PWA as Android Application

### bubblewrap

##### About
- wrap the application in Trusted Web Activity (a kind of activity in android)
- needs digital assets link verification
- bubblewrap is the backend of PWABuilder.com (the backend seems to be run by Microsoft in Azure cloud?)

##### Source
- https://github.com/GoogleChromeLabs/bubblewrap
- https://github.com/GoogleChromeLabs/bubblewrap/tree/master/packages/cli
- https://developers.google.com/web/android/trusted-web-activity/quick-start

##### Note
- install node.js and android studio
- run ```npm i -g @bubblewrap/cli``` (please see link above to install without sudo, OR just chmod /usr/lib/node_modules/ and /usr/bin/ add o+w, after installation turn it back to o-w)
- create and enter new directory/folder
- run ```bubblewrap init --manifest https://your.web.com/some/optional/directory/manifest.json```
- open the resulting directory in android studio, then publish as signed apk
- see documentation above for other commands

### svgomg-twa

##### About
- proof of concept/prototype of wrapping in Trusted Web Activity
- needs digital assets link verification

##### Source
- https://dev.to/jumpalottahigh/how-to-publish-a-pwa-on-the-google-play-store-2bid
- https://github.com/GoogleChromeLabs/svgomg-twa

##### Note
- git clone https://github.com/GoogleChromeLabs/svgomg-twa
- edit app/src/build.gradle
- add icons
- open the resulting directory in android studio, then publish as signed apk

### xtools-at/Android-PWA-Wrapper

##### Source
- https://github.com/xtools-at/Android-PWA-Wrapper

##### Note
- git clone https://github.com/xtools-at/Android-PWA-Wrapper
- the maintainer also have iOS and Electron wrapper
- open the resulting directory in android studio
- follow tutorial in README.md (edit some config, change some icons)
- result in small (1.4mb) apk
- wrapped in WebView rather than TrustedWebActivity


## Trusted Web Activity
- TWA will show notification "Running in Chrome" "You'll see your web.domain.here.com sign-in status, browsing data, and site data in Chrome"
- on older android version it might show "toast" or "snackbar" saying "running in chrome"
- result in very small apk file (900kb)
- PWA with custom install button will not show the install button if it's already installed as TWA (won't fire ```beforeinstallprompt```)

### Digital Assets Link Verification
- Trusted Web Activity needs to be verified so that it can be 'Trusted'
- process: create keystore file in android studio (default location is ```~/.android/keystore.jks```)
- process: note/keep/write the keystore's KEY_PASSWORD, KEYSTORE_PASSWORD, FILE_PATH, ALIAS; note that there's two password to be remembered here
- process: view it's fingerprint: ```keytool -list -v -keystore YOUR_KEYSTORE_FILE_PATH -alias YOUR_ALIAS -storepass YOUR_KEYSTORE_PASSWORD -keypass YOUR_KEY_PASSWORD```, example output:
```
Alias name: LaptopDevelopment
Creation date: Dec 2, 2000
Entry type: PrivateKeyEntry
Certificate chain length: 1
Certificate[1]:
Owner: CN=Kristian Tanuwijaya, OU=me, O=me, L=me, ST=me, C=US
Issuer: CN=Kristian Tanuwijaya, OU=me, O=me, L=me, ST=me, C=US
Serial number: 74a431ac
Valid from: Wed Dec 02 09:34:43 2000 until: Sun Nov 26 09:34:43 2999
Certificate fingerprints:
         SHA1: D8:D6:FF:7A:54:41:99:8A:51:4D:D9:BD:50:76:EA:E9:EC:DF:64:0D
         SHA256: 5E:8E:29:ED:3C:DA:C8:17:AA:06:CB:75:1D:51:D4:FC:4A:6A:BF:9F:DC:6E:4F:09:C5:40:E9:58:38:CE:68:F8
Signature algorithm name: SHA256withRSA
Subject Public Key Algorithm: 2048-bit RSA key
Version: 3

Extensions: 

#1: ObjectId: 2.5.29.14 Criticality=false
SubjectKeyIdentifier [
KeyIdentifier [
0000: 9D 63 AD 1C D5 C8 E6 59   B0 C8 D9 AD 88 3A 8D 4F  .c.....Y.....:.O
0010: 5D B1 5D EC                                        ].].
]
]
```
- process: take note of SHA256 fingerprint
- process: make ```https://web.domain.here.com/.well-known/assetlinks.json``` return 200 and this kind of text:
```json
[{
  "relation": ["delegate_permission/common.handle_all_urls"],
  "target": {
    "namespace": "android_app",
    "package_name": "com.here.domain.web.myapp",
    "sha256_cert_fingerprints":
    ["5E:8E:29:ED:3C:DA:C8:17:AA:06:CB:75:1D:51:D4:FC:4A:6A:BF:9F:DC:6E:4F:09:C5:40:E9:58:38:CE:68:F8"]
  }
},
{
  "relation": ["delegate_permission/common.get_login_creds"],
  "target": {
    "namespace": "web",
    "site": "https://web.domain.here.com"
  }
},
{
  "relation": ["delegate_permission/common.get_login_creds"],
  "target": {
    "namespace": "android_app",
    "package_name": "web.domain.here.com",
    "sha256_cert_fingerprints":
    ["5E:8E:29:ED:3C:DA:C8:17:AA:06:CB:75:1D:51:D4:FC:4A:6A:BF:9F:DC:6E:4F:09:C5:40:E9:58:38:CE:68:F8"]
  }
}]
```
- the file tells the android OS that ```web.domain.here.com``` authorize application with package name ```com.here.domain.web.myapp``` to open links inside it as trusted web activity, if the application is signed with key with signature defined in ```sha256_cert_fingerprints```
- there seems to be a lot of pitfalls (windows line ending / CF LF, byte-order mark / BOM, SSL certificate, HTTP code must be 200, etc.)
- tools to help developing/debugging digital assets link verification:
    - debugging if signature verification failed: ```adb logcat -v brief | grep -e OriginVerifier -e digital_asset_links```
    - checking if google _approves_ your DALV: https://digitalassetlinks.googleapis.com/v1/statements:list?source.web.site=https://web.domain.here.com&relation=delegate_permission/common.handle_all_urls
    - android studio -> Tools -> App Links Assistant
- sources:
    - https://developer.android.com/training/app-links/verify-site-associations
    - https://developers.google.com/digital-asset-links/v1/create-statement

