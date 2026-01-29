import { Component, inject } from '@angular/core';
import { QuizCardComponent } from "../../shared/ui/quiz-card/quiz-card.component";
import { QuizService } from '../../core/services/quiz.service';

@Component({
  selector: 'app-quiz-list',
  imports: [QuizCardComponent],
  templateUrl: './quiz-list.component.html',
  styleUrl: './quiz-list.component.scss',
})
export class QuizListComponent {
  quizService = inject(QuizService);
  
}
