import { Component, input } from '@angular/core';
import { Quiz } from '../../models/quiz.model';
import { RouterLink } from "@angular/router";

@Component({
  selector: 'app-quiz-card',
  imports: [RouterLink],
  templateUrl: './quiz-card.component.html',
  styleUrl: './quiz-card.component.scss',
})
export class QuizCardComponent {
  quiz = input.required<Quiz>();
}
