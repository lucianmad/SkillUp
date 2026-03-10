import { Component, inject, OnInit, signal } from '@angular/core';
import { QuizService } from '../../core/services/quiz.service';
import { QuizCardComponent } from '../../shared/ui/quiz-card/quiz-card.component';
import { QuizListResponse } from '../../shared/models/quiz-player.model';

@Component({
  selector: 'app-home',
  imports: [QuizCardComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent implements OnInit{
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