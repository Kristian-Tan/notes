# About Progressive Web Application

## History
- drafted in 2016 by Google+Microsoft

## Capability
- basically everything that web application can do
    - listing all kind of web API capability that your browser can do: https://whatwebcando.today/
    - generic sensor (a.k.a. accelerometer), web API demo: https://intel.github.io/generic-sensor-demos/
    - webassembly (running c++/rust/compiled language) in web like openCV
    - generating/scanning QR (camera) https://www.sitepoint.com/create-qr-code-reader-mobile-website/ https://nimiq.github.io/qr-scanner/demo/
    - push notification (even when browser/app closed, example: firebase), demo: https://gauntface.github.io/simple-push-demo/ (firebase, triggerred by custom HTTP request to firebase server)
    - detect device going online/offline
    - access file on device / filesystem API https://web.dev/file-system-access/
    - geolocation (like google maps)
- offline/caching capability through service worker javascript
- installable in multi platform (in android, iphone/ipad, windows, mac)
- publishable to application markets (google playstore, microsoft store, but apple app store still needs some workaround)
- other extensive capability that's currently developed/drafted
    - share (intent) https://web.dev/web-share/ https://web.dev/web-share-target/
    - being target of a sharing (intent)
    - background geolocation (not officially supported yet!) https://github.com/RichardMaher/Brotkrumen
    - app badge (number of notification on corner of launcher icon) https://web.dev/badging-api/
    - disable pull down to refresh/reload https://stackoverflow.com/questions/36212722/how-to-prevent-pull-down-to-refresh-of-mobile-chrome
    - disable back button to close app https://stackoverflow.com/questions/43329654/android-back-button-on-a-progressive-web-application-closes-de-app

## What is it
- "vanilla" *web application* (can be static html or with server side)
- PLUS *manifest*, a JSON formatted text file for specifying application identity (name, description, icon, color, etc.)
- PLUS *service worker*, a javascript event-driven program that also act as proxy for each request

## Minimal requirement to be installable
- manifest.json
- registering service worker
- serviceWorker.js must be on root directory of the web (not inside ./dist/js or other directory), then declare scope
- use https with valid certificate (not self signed, use qualified domain name), important since service worker will act as proxy for requests
- example: https://Kristian-Tan.github.io/pwa-minimal

## Recommendations, best practices (optional), and neat features:
- html head meta tags
- icons (square at 16, 32, 192, 256, 512, and 192 non-transparent png for apple app icon), favicon
- cache offline pages (select files/html/js/css to be copied to cache and servied when offline in serviceWorker.js)
- save last user's state (save user's credential, last page accessed, unfinished edit)
- choose cache strategy suited with nature of the app https://blog.bitsrc.io/5-service-worker-caching-strategies-for-your-next-pwa-app-58539f156f52
- use responsive web design

