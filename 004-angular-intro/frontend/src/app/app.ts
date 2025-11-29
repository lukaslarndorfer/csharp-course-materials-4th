import {Component, inject, signal, WritableSignal} from '@angular/core';
import {RouterLink, RouterOutlet} from '@angular/router';
import {MatButton, MatIconButton} from '@angular/material/button'
import {MatSidenav, MatSidenavContainer, MatSidenavContent} from '@angular/material/sidenav';
import {AsyncPipe} from '@angular/common';
import {MatToolbar} from '@angular/material/toolbar';
import {MatListItem, MatNavList} from '@angular/material/list';
import {MatIcon} from '@angular/material/icon';
import {BreakpointObserver, Breakpoints} from '@angular/cdk/layout';
import {firstValueFrom, map, Observable, shareReplay} from 'rxjs';
import {NavItem} from './shared/nav-item/nav-item';
import {AuthService} from '../core/services/auth-service';
import {SnackbarService} from '../core/services/snackbar-service';
import {MatDialog, MatDialogRef} from '@angular/material/dialog';
import {LoginDialog, LoginDialogResult} from './login-dialog/login-dialog';

@Component({
  selector: 'app-root',
  imports: [
    MatSidenavContainer,
    MatSidenav,
    AsyncPipe,
    MatToolbar,
    MatNavList,
    MatSidenavContent,
    MatIconButton,
    MatIcon,
    RouterOutlet,

    NavItem
  ],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = 'Tank Museum';
  private readonly breakpointObserver: BreakpointObserver = inject(BreakpointObserver);
  protected readonly isHandset$: Observable<boolean> = this.breakpointObserver
    .observe(Breakpoints.Handset)
    .pipe(
      map(result => result.matches),
      shareReplay({bufferSize: 1, refCount: true})
    );
  protected readonly isNotHandset$ = this.isHandset$
    .pipe(
      map(isHandset => !isHandset),
      shareReplay({bufferSize: 1, refCount: true})
    );
  protected readonly authService: AuthService = inject(AuthService);
  private readonly snackbar: SnackbarService = inject(SnackbarService);
  private readonly dialog: MatDialog = inject(MatDialog);

  protected async handleLogin(event: MouseEvent | undefined = undefined)
    : Promise<void> {
    if (event) {
      event.preventDefault(); // normalerweise postet er
    }
    const dialogRef = this.dialog.open(LoginDialog);
    const resultObservable = dialogRef.afterClosed();
    const result = await firstValueFrom(resultObservable) as LoginDialogResult;
    if (!result || !result.success) {
      if (!result.cancelled) {
        this.snackbar.show(`Login failed`);

      }
      return;
    }
    this.snackbar.show(`Successfully logged in!`);
  }
}
