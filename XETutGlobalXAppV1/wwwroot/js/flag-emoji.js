customElements.define('flag-emoji', class extends HTMLElement {

	/**
	 * Create a new instance
	 */
	constructor() {
		super();
		this.textContent = this.getFlagEmoji(this.getAttribute('code'));
	}

	/**
	 * Get the flag emoji for the country
	 * @link https://dev.to/jorik/country-code-to-flag-emoji-a21
	 * @param  {String} countryCode The country code
	 * @return {String}             The flag emoji
	 */
	getFlagEmoji(countryCode) {
		let codePoints = countryCode.toUpperCase().split('').map(char => 127397 + char.charCodeAt());
		return String.fromCodePoint(...codePoints);
	}

});