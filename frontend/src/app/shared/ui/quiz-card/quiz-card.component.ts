import { Component, input } from '@angular/core';
import { Quiz } from '../../models/quiz.model';

@Component({
  selector: 'app-quiz-card',
  imports: [],
  templateUrl: './quiz-card.component.html',
  styleUrl: './quiz-card.component.scss',
})
export class QuizCardComponent {
  quiz = input.required<Quiz>();

}
