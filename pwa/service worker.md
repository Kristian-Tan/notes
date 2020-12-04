# Service Worker for Progressive Web Application

- view all chrome's service worker (and inspect/start/stop it) chrome://serviceworker-internals/?devtools

## Caching Strategy
- documentation of "fetch()" https://developer.mozilla.org/en-US/docs/Web/API/Fetch_API
- documentation of "window.caches CacheStorage" https://developer.mozilla.org/en-US/docs/Web/API/CacheStorage
- documentation of "Cache class" https://developer.mozilla.org/en-US/docs/Web/API/Cache

### Examples
- these examples are for generic use case, depending on nature of the app, writing custom caching strategy / writing custom service worker might be preferred

##### Example 1: 5 commonly used strategy
- source: https://blog.bitsrc.io/5-service-worker-caching-strategies-for-your-next-pwa-app-58539f156f52
```js
self.addEventListener('fetch', function(event) {
    var request = event.request;
    event.respondWith(
        // for converting PHP based webapp, use network first

        //* // network first: try to fetch response from network. if succeeds, return response. if network fails, falls back to cache
        fetch(event.request).catch(function() {
            return caches.match(event.request)
        })
        // */

        /* // network only: solely uses the network to fetch and serve the response. does not fallback to any cache
        fetch(event.request).then(function(networkResponse) {
            return networkResponse
        }) // */

        /* // cache only: responds from the cache only. does not fall back to network
        caches.open(cacheName).then(function(cache) {
            cache.match(event.request).then(function(cacheResponse) {
                return cacheResponse;
            })
        })
        // */

        /* // cache first: look for a response in the cache first, if response is found previously cached, then serve the cache. if not found it will fetch the response from network, serve it, and cache it for next time.
        caches.match(request).then(function(response) {
            if (response) {
                return response;
            }
            return fetch(request).then(function(response) {
                var responseToCache = response.clone();
                caches.open(cacheName).then(function(cache) {
                    cache.put(request, responseToCache).catch(function(err) {
                        console.warn(request.url + ': ' + err.message);
                    });
                });
                return response;
            });
        }) // */

        /* //
        caches.open(cacheName).then(function(cache) {
            cache.match(event.request).then( function(cacheResponse) {
                fetch(event.request).then(function(networkResponse) {
                    cache.put(event.request, networkResponse)
                })
                return cacheResponse || networkResponse;
            })
        }) 
        // */
    );
});
```

### Example 2: generic network first strategy for converting PHP apps to PWA
- will cache ALL response (warning: your application data/cache will/may grow very big over time, it might annoy some user)
- when user visit a page, fetch response from network, but when user goes offline it will show cached page
- when user goes offline and page not available in cache, show browser's default error page (not a good UI/UX)
```js
self.addEventListener('fetch', function (event) {
    event.respondWith(
        fetch(event.request)
        .then(function(response){
            var responseCopy = response.clone();
            caches.open(cacheName).then(function (cache) {
                cache.put(event.request, responseCopy);
            });
            return response;
        })
        .catch(function(err) {
            return caches.match(event.request);
        })
    )
})
```

### Example 3: generic network first strategy for converting PHP apps to PWA, WITH custom 'offline' page
- will cache ALL response (warning: your application data/cache will/may grow very big over time, it might annoy some user)
- when user visit a page, fetch response from network, but when user goes offline it will show cached page
- when user goes offline and page not available in cache, show a custom error/offline page
- the custom error/offline page MUST be pre-cached (it will show browser's default error page if custom error/offline page NOT in cache)
- in this case, let's say the custom error/offline page is called ```./offline``` (inside current/service worker's context) 
```js
self.addEventListener('install', function(event) {
    event.waitUntil(
        caches.open(cacheName)
        .then(function(cache) {
            return cache.addAll(["offline"]) // pre-cache offline page (put your offline page path here)
            .then(function() {
                return self.skipWaiting();
            })
            .catch(function(error) {
                console.error('Failed to cache', error);
            })
        })
    );
});
self.addEventListener('fetch', function (event) {
    event.respondWith( // when fetch event happen,
        fetch(event.request) // fetch resource from network (ex: make real HTTP request to webserver)
        .then(function(response){ // when the request is fulfilled / success
            var responseCopy = response.clone(); // clone/copy the response (since response cannot be used twice, it have to be cloned/copied explicitly)
            caches.open(cacheName).then(function (cache) { // open cache
                cache.put(event.request, responseCopy); // put the copied/cloned response in cache
            });
            return response; // respond the request with the real response
        })
        .catch(function(err) { // when the request is not fulfilled / failed to resolve HTTP request
            return caches.match(event.request) // seek the cache for matching response (check if the request was previously cached or not)
            .then(function(response) { // note: seeking cache manager with caches.match() will never error/catch, but if cache not found the result/response will be undefined
                if (response !== undefined) { // if matching cache found in cache manager (cache hit), return/respond to the request with cached content
                    return response;
                } else { // if no matching cache found in cache manager (cache miss),
                    return caches.match("offline") // seek the cache manager for your custom offline page (put your offline page path here)
                    .then(function(responseOffline) {
                        return responseOffline; // return/respond to the request with cached-custom offline page
                    });
                }
            });
        })
    )
})
```


