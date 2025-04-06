import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { UserLoginDto } from '../../models/user.model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  loginForm = this.fb.group({
    username: ['', Validators.required],
    password: ['', Validators.required]
  });

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {}

  onSubmit() {
    if (this.loginForm.valid) {
      const userDto: UserLoginDto = {
        username: this.loginForm.value.username,
        password: this.loginForm.value.password
      };
      this.authService.login(userDto).subscribe({
        next: () => this.router.navigate(['/dashboard']),
        error: (err) => console.error('Login failed:', err)
      });
    }
  }
}