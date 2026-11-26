const languages = ['en', 'lv', 'ru'];

// 400 days. Chrome, Edge, and Firefox cap a cookie Max-Age above that.
// Safari deletes a cookie written with document.cookie after 7 days of browser use without a visit.
// Sources:
// - https://developer.chrome.com/blog/cookie-max-age-expires
// - https://bugzilla.mozilla.org/show_bug.cgi?id=1964943
// - https://webkit.org/tracking-prevention/
const informationCookieMaxAgeSeconds = 34560000;

const informationCookieDomain = document.body.dataset.informationCookieDomain === null
	? ''
	: `Domain=${document.body.dataset.informationCookieDomain}`

const languageSelect = document.getElementById('language');
const menu = document.getElementById('navbar-menu');
const menuButton = document.querySelector('.menu-button');

function saveInformationCookie(cookieName, cookieValue) {
	document.cookie =
		`${cookieName}=${cookieValue};` +
		` Path=/;` +
		` Max-Age=${informationCookieMaxAgeSeconds};` +
		` SameSite=Lax;` +
		` Secure;` +
		` ${informationCookieDomain}`;
}

const darkThemeCheckbox = document.getElementById('dark-theme');
darkThemeCheckbox.checked = document.documentElement.dataset.theme === 'dark';

languageSelect.addEventListener('change', () => {
	const language = languageSelect.value;

	if (!languages.includes(language)) {
		return;
	}

	saveInformationCookie('language', language);
	const segments = location.pathname.split('/');

	if (languages.includes(segments[1])) {
		segments[1] = language;
	} else {
		segments.splice(1, 0, language);
	}

	location.assign(`${segments.join('/')}${location.search}${location.hash}`);
});

darkThemeCheckbox.addEventListener('change', () => {
	const theme = darkThemeCheckbox.checked ? 'dark' : 'light';
	document.documentElement.dataset.theme = theme;
	saveInformationCookie('theme', theme);
});

menuButton.addEventListener('click', () => {
	const open = menu.classList.toggle('open');
	menuButton.classList.toggle('default', open);
	menuButton.setAttribute('aria-expanded', String(open));
});
