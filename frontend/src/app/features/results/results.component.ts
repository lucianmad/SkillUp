import { Component, inject, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-results',
  imports: [RouterLink],
  templateUrl: './results.component.html',
  styleUrl: './results.component.scss',
})
export class ResultsComponent {
  private router = inject(Router);
  
  score = signal(0);
  maximumScore = signal(0);
  feedbackMessage = signal('');

  ngOnInit() {
    const state = history.state as { score?: number, total?: number };

    if (state && state.total != undefined) {
      this.score.set(state.score!);
      this.maximumScore.set(state.total!);

      const percentage = (state.score! / state.total!) * 100;

      if (percentage > 80) {
        this.feedbackMessage.set('🏆 Master!');
      }
      else if (percentage > 50) {
        this.feedbackMessage.set('👍 Good job, but you need some improvements!');
      }
      else {
        this.feedbackMessage.set('📚 Keep practicing!');
      }
    }
    else {
      this.router.navigate(['/']);
    }
  }
}
