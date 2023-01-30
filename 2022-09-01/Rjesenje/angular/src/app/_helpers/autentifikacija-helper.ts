import { LoginInformacije } from './login-informacije';

export class AutentifikacijaHelper {
  static setLoginInfo(token: LoginInformacije): void {
    if (token == null) token = new LoginInformacije();
    localStorage.setItem('autentifikacija-token', JSON.stringify(token));
  }

  static getLoginInfo(): LoginInformacije {
    const token = localStorage.getItem('autentifikacija-token');
    if (token === '') return new LoginInformacije();
    try {
      const loginInformacije: LoginInformacije = JSON.parse(token);
      if (loginInformacije == null) return new LoginInformacije();
      return loginInformacije;
    } catch (e) {
      return new LoginInformacije();
    }
  }
}
