export interface LoginCommand {
  email: string;
  password: string;
}

export interface RegisterCommand {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
}

export interface RefreshCommand {
  refreshToken: string;
}

export interface LogoutCommand {
  refreshToken: string;
}

export interface LoginResponse {
  token: string;
  refreshToken: string;
  email: string;
  role: string;
  expiresAt: string;
  refreshTokenExpiresAt: string;
}

export type AuthRole = 'ADMIN' | 'USER' | string;

export interface AuthUser {
  email: string;
  role: string;
  expiresAt: string;
}