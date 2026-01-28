import { Component, inject } from '@angular/core';
import { QuizService } from '../../core/services/quiz.service';
import { ActivatedRoute, RouterLink } from '@angular/router';

@Component({
  selector: 'app-results',
  imports: [RouterLink],
  templateUrl: './results.component.html',
  styleUrl: './results.component.scss',
})
export class ResultsComponent {
  quizService = inject(QuizService);
  private route = inject(ActivatedRoute);
  score: number = 0;
  maximumScore: number = 0;
  feedbackMessage: string = '';

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.score = this.quizService.computeResults(id); 
      
      const quiz = this.quizService.activeQuiz();
      this.maximumScore = quiz ? quiz.questions.length : 0;

      const percentage = (this.score / this.maximumScore) * 100;

      if (percentage > 80) {
        this.feedbackMessage = '🏆 Master!';
      }
      else if (percentage > 50) {
        this.feedbackMessage = '👍 Good job, but you need some improvements!'
      }
      else {
        this.feedbackMessage = '📚 Keep practicing!';
      }
    }
  }
}
