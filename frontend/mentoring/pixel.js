function getSessionId() {
    let sessionId = sessionStorage.getItem("sessionId");
    if (!sessionId) {
        sessionId = 'session-' + crypto.randomUUID().toString();
        sessionStorage.setItem("sessionId", sessionId);
    }
    return sessionId;
}

function getUserId() {
    let userId = localStorage.getItem("userId");
    if (!userId) {
        userId = 'user-' + crypto.randomUUID().toString();
        localStorage.setItem("userId", userId);
    }
    return userId;
}

function sendPixelEvent(trackingUrl, eventType, elementId, interactionDetails = {}) {
    const pixelData = {
        eventType: eventType,
        elementId: elementId,
        timestamp: new Date().toISOString(),
        userId: getUserId(),
        sessionId: getSessionId(),
        pageUrl: window.location.href,

        additionalData: {
            screenWidth: window.screen.width,
            screenHeight: window.screen.height,
            viewportWidth: window.innerWidth,
            viewportHeight: window.innerHeight,

            userAgent: navigator.userAgent,
            referrer: document.referrer,
            campaign: getUTMParameter('campaign'),
            utmSource: getUTMParameter('utm_source'),
            utmMedium: getUTMParameter('utm_medium'),
            utmCampaign: getUTMParameter('utm_campaign'),
            interactionDetails: interactionDetails
        }
    };

    fetch(trackingUrl, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(pixelData)
    }).catch(error => {
        console.log(pixelData);
        console.error("Pixel event failed:", error)
    });
}

function getUTMParameter(param) {
    const urlParams = new URLSearchParams(window.location.search);
    return urlParams.get(param) || '';
}
