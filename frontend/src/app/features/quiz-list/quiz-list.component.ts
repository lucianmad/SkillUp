import { Component, inject, OnInit, signal } from '@angular/core';
import { QuizCardComponent } from "../../shared/ui/quiz-card/quiz-card.component";
import { QuizService } from '../../core/services/quiz.service';
import { QuizListResponse } from '../../shared/models/quiz-player.model';

@Component({
  selector: 'app-quiz-list',
  imports: [QuizCardComponent],
  templateUrl: './quiz-list.component.html',
  styleUrl: './quiz-list.component.scss',
})
export class QuizListComponent implements OnInit {
  private quizService = inject(QuizService);

  quizzes = signal<QuizListResponse[]>([]);

  ngOnInit(): void {
    this.quizService.getQuizzes().subscribe({
      next: (data) => {
        this.quizzes.set(data);
      },
      error: (err) => {

      }
    })
  }
}
