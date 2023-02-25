using FluentValidation;

namespace ThreadboxApi.Dtos
{
	public class PaginationParamsDto
	{
		public int PageIndex { get; set; }
		public int PageSize { get; set; }

		public class Validator : AbstractValidator<PaginationParamsDto>
		{
			public Validator()
			{
				RuleFor(x => x.PageIndex).GreaterThanOrEqualTo(0);
				RuleFor(x => x.PageSize).GreaterThan(0);
			}
		}
	}
}