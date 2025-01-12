if (document.cookie.includes('visitor_id') === false) {
    const array = new Uint8Array(64);
    window.crypto.getRandomValues(array);

    const visitorId = Array.from(array, byte => byte.toString(16).padStart(2, '0')).join('');
    const expirationDate = new Date();
    expirationDate.setFullYear(expirationDate.getFullYear() + 1);

    document.cookie = `visitor_id=${visitorId}; Path=/; Secure; SameSite=Strict; Expires=${expirationDate.toUTCString()}`;
}
