import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { QuizService } from '../../core/services/quiz.service';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { QuestionListResponse, SubmitAnswerItem, SubmitAnswersRequest } from '../../shared/models/quiz-player.model';

@Component({
  selector: 'app-quiz-taker',
  imports: [RouterLink],
  templateUrl: './quiz-taker.component.html',
  styleUrl: './quiz-taker.component.scss',
})
export class QuizTakerComponent implements OnInit {
  private quizService = inject(QuizService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  quizId = "";

  questions = signal<QuestionListResponse[]>([]);
  currentIndex = signal(0);
  userAnswers = signal<Record<string, string[]>>({});

  currentQuestion = computed(() => {
    const questions = this.questions();
    return questions.length > 0 ? questions[this.currentIndex()] : null;
  });

  isLastQuestion = computed(() => {
    return this.questions().length > 0 && this.currentIndex() === this.questions().length - 1;
  })

  ngOnInit(): void {
    this.quizId = this.route.snapshot.paramMap.get("id") || "";
    
    if (this.quizId) {
      this.quizService.getQuestions(this.quizId).subscribe({
        next: (data) => {
          this.questions.set(data);
        },
        error: (err) => {
          console.error("Failed to fetch the questions", err);
        }
      });
    }
  }

  toggleAnswer(answerId: string) {
    const question = this.currentQuestion();
    if (!question) return;

    this.userAnswers.update(state => {
      const currentlySelected = state[question.id] || [];
      let newSelection: string[];

      if (question.type === 'SingleChoice') {
        newSelection = [answerId];
      } 
      else {
        if (currentlySelected.includes(answerId)) {
          newSelection = currentlySelected.filter(id => id !== answerId);
        }
        else {
          newSelection = [...currentlySelected, answerId];
        }
      }

      return { ...state, [question.id]: newSelection };
    })
  }

  isSelected(answerId: string): boolean {
    const question = this.currentQuestion();
    if (!question) return false;

    const selected = this.userAnswers()[question.id] || [];
    return selected.includes(answerId);
  }

  nextQuestion() {
    if (!this.isLastQuestion()) {
      this.currentIndex.update(i => i + 1);
    }
  }

  prevQuestion() {
    if (this.currentIndex() > 0) {
      this.currentIndex.update(i => i - 1);
    }
  }

  submitQuiz() {
    const payload: SubmitAnswersRequest = {
      answers: Object.entries(this.userAnswers()).map(([qId, aIds]) => {
        const item: SubmitAnswerItem = {
          questionId: qId,
          selectedAnswersId: aIds
        };
        return item;
      })
    };

    this.quizService.submitAnswers(this.quizId, payload).subscribe({
      next: (response) => {
        this.router.navigate(['/quiz', this.quizId, 'results'], {
          state: {
            score: response.score,
            total: response.totalQuestions
          }
        });
      },
      error: (err) => {
        console.error('Submission failed', err);
      }
    })
  }
}
