export interface UserRegistrationDto {
  username: string;
  email: string;
  password: string;
}

export interface UserLoginDto {
  username: string;
  password: string;
}

export interface User {
  id: string;
  username: string;
  email: string;
  role: string;
}