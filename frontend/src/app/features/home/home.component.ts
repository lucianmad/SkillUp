import { Component, inject } from '@angular/core';
import { QuizService } from '../../core/services/quiz.service';
import { QuizCardComponent } from '../../shared/ui/quiz-card/quiz-card.component';

@Component({
  selector: 'app-home',
  imports: [QuizCardComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent {
  quizService = inject(QuizService);
}