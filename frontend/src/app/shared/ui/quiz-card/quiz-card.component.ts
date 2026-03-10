import { Component, input } from '@angular/core';
import { RouterLink } from "@angular/router";
import { QuizListResponse } from '../../models/quiz-player.model';

@Component({
  selector: 'app-quiz-card',
  imports: [RouterLink],
  templateUrl: './quiz-card.component.html',
  styleUrl: './quiz-card.component.scss',
})
export class QuizCardComponent {
  quiz = input.required<QuizListResponse>();
}
